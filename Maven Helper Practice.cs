using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MavenHelper
{
    public partial class Maven_Helper_Practice : Form
    {
        SpeechRecognitionEngine _recog = new SpeechRecognitionEngine();
        SpeechSynthesizer Maven = new SpeechSynthesizer();
        ArrayList grammarRules = new ArrayList();
        string[] grammar;

        ListnerStates ls = ListnerStates.LS_INACTIVE;
        ArrayList left = new ArrayList();
        ArrayList right = new ArrayList();
        ArrayList bottom = new ArrayList();
        ArrayList display = new ArrayList();
        ArrayList scurry = new ArrayList();
        ArrayList nextprompt = new ArrayList();
        ArrayList resetCommands = new ArrayList();
        bool cbDisplayChecked, cbScurryChecked;

        ArrayList steps = new ArrayList();
        int step;
        int maxStages, counter = 0, maxTime;
        int[] directions;
        bool scurryPhaseActive = false;
        CommandType lastCommand = CommandType.CT_INVALID;

        public Maven_Helper_Practice()
        {
            InitializeComponent();
        }

        private void Maven_Helper_Practice_Load(object sender, EventArgs e)
        {
            FileStream commandsFile = File.Open("Commands.dat", FileMode.Open);
            BinaryFormatter reader = new BinaryFormatter();
            left = (ArrayList)reader.Deserialize(commandsFile);
            right = (ArrayList)reader.Deserialize(commandsFile);
            bottom = (ArrayList)reader.Deserialize(commandsFile);
            display = (ArrayList)reader.Deserialize(commandsFile);
            scurry = (ArrayList)reader.Deserialize(commandsFile);
            nextprompt = (ArrayList)reader.Deserialize(commandsFile);
            resetCommands = (ArrayList)reader.Deserialize(commandsFile);
            cbDisplayChecked = (bool)reader.Deserialize(commandsFile);
            cbScurryChecked = (bool)reader.Deserialize(commandsFile);
            commandsFile.Close();

            _recog.SetInputToDefaultAudioDevice();
            RefreshCommands();
            _recog.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(_recog_SpeechRecognized);
            _recog.RecognizeAsync(RecognizeMode.Multiple);
            Maven.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);

            maxStages = Int32.Parse(tbStages.Text);
        }

        private void RefreshCommands()
        {
            grammarRules.Clear();
            foreach (string item in left)
                grammarRules.Add(item);
            foreach (string item in right)
                grammarRules.Add(item);
            foreach (string item in bottom)
                grammarRules.Add(item);
            foreach (string item in display)
                grammarRules.Add(item);
            foreach (string item in scurry)
                grammarRules.Add(item);
            foreach (string item in nextprompt)
                grammarRules.Add(item);

            grammar = (string[])grammarRules.ToArray(typeof(string));
            _recog.UnloadAllGrammars();
            _recog.LoadGrammar(new Grammar(new GrammarBuilder(new Choices(grammar))));
            
        }

        private CommandType SpeechToCommand(string Speech)
        {
            Speech = Speech.ToLower();
            foreach (string item in left)
                if (Speech.Equals(item))
                    return CommandType.CT_LEFT;

            foreach (string item in right)
                if (Speech.Equals(item))
                    return CommandType.CT_RIGHT;

            foreach (string item in bottom)
                if (Speech.Equals(item))
                    return CommandType.CT_BOTTOM;

            foreach (string item in display)
                if (Speech.Equals(item))
                    return CommandType.CT_DISPLAY;

            foreach (string item in scurry)
                if (Speech.Equals(item))
                    return CommandType.CT_SCURRY;

            foreach (string item in nextprompt)
                if (Speech.Equals(item))
                    return CommandType.CT_NEXT;

            foreach (string item in resetCommands)
                if (Speech.Equals(item))
                    return CommandType.CT_RESET;

            return CommandType.CT_INVALID;
        }

        private void btMore_Click(object sender, EventArgs e)
        {
            int stages = Int32.Parse(tbStages.Text);
            if (stages < 10)
                stages++;
            tbStages.Text = stages.ToString();
            maxStages = stages;
        }

        private void btLess_Click(object sender, EventArgs e)
        {
            int stages = Int32.Parse(tbStages.Text);
            if (stages > 0)
                stages--;
            tbStages.Text = stages.ToString();
            maxStages = stages;
        }

        private void tScurry_Tick(object sender, EventArgs e)
        {
            int pbValue = pbTimer.Value - (int)(100 / maxTime);
            if (pbValue <= 0)
            {
                pbTimer.Value = 0;
                GameOver();
            }

            else
                pbTimer.Value = pbValue;
        }

        private void GameOver(bool winner = false)
        {
            scurryPhaseActive = false;
            counter = 0;
            lbLeft.BackColor = Color.Blue;
            lbRight.BackColor = Color.Blue;
            lbBottom.BackColor = Color.Blue;
            tScurry.Stop();

            if (!winner)
                Microsoft.VisualBasic.Interaction.MsgBox("You're dead, hopefully your logout skills are better than your memory!", Microsoft.VisualBasic.MsgBoxStyle.Information, "Maven Helper");
            else
                Microsoft.VisualBasic.Interaction.MsgBox("Good job with the memory game! You still need DPS to beat the rest of the fight!", Microsoft.VisualBasic.MsgBoxStyle.Information, "Maven Helper");
        }

        private void lbLeft_Click(object sender, EventArgs e)
        {
            if (!scurryPhaseActive) return;

            int correctZone = directions[counter];
            if (correctZone == 0)
            {
                lbLeft.BackColor = Color.Green;
                lbRight.BackColor = Color.Blue;
                lbBottom.BackColor = Color.Blue;
                counter++;

                if (counter == maxStages)
                    GameOver(true);
            } else
            {
                lbLeft.BackColor = Color.Red;
                lbRight.BackColor = Color.Blue;
                lbBottom.BackColor = Color.Blue;
                GameOver();
            }
        }

        private void lbBottom_Click(object sender, EventArgs e)
        {
            if (!scurryPhaseActive) return;

            int correctZone = directions[counter];
            if (correctZone == 2)
            {
                lbLeft.BackColor = Color.Blue;
                lbRight.BackColor = Color.Blue;
                lbBottom.BackColor = Color.Green;
                counter++;

                if (counter == maxStages)
                    GameOver(true);
            }
            else
            {
                lbLeft.BackColor = Color.Blue;
                lbRight.BackColor = Color.Blue;
                lbBottom.BackColor = Color.Green;
                GameOver();
            }
        }

        private void lbRight_Click(object sender, EventArgs e)
        {
            if (!scurryPhaseActive) return;

            int correctZone = directions[counter];
            if (correctZone == 1)
            {
                lbLeft.BackColor = Color.Blue;
                lbRight.BackColor = Color.Green;
                lbBottom.BackColor = Color.Blue;
                counter++;

                if (counter == maxStages)
                    GameOver(true);
            }
            else
            {
                lbLeft.BackColor = Color.Blue;
                lbRight.BackColor = Color.Red;
                lbBottom.BackColor = Color.Blue;
                GameOver();
            }
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            directions = new int[maxStages];         
            Random r = new Random();
            int dir, lastdir = -1;

            for (int i = 0; i < maxStages; i++)
            {
                dir = r.Next(0, 3);
                while (dir == lastdir)
                    dir = r.Next(0, 2);
                directions[i] = dir;
                lastdir = dir;
            }
            maxTime = maxStages * 3;
            pbTimer.Value = 100;
            scurryPhaseActive = false;
            tDisplay.Start();
        }

        private void Maven_Helper_Practice_FormClosed(object sender, FormClosedEventArgs e)
        {
            _recog.RecognizeAsyncStop();
            resetSteps();
            tDisplay.Stop();
            tScurry.Stop();
            this.Dispose();
        }

        private void tDisplay_Tick(object sender, EventArgs e)
        { 
            if (counter == maxStages)
            {
                counter = 0;
                scurryPhaseActive = true;
                lbLeft.BackColor = Color.Blue;
                lbRight.BackColor = Color.Blue;
                lbBottom.BackColor = Color.Blue;
                tScurry.Start();
                tDisplay.Stop();
                return;
            }

            if (directions[counter] == 0)
            {
                lbLeft.BackColor = Color.Yellow;
                lbRight.BackColor = Color.Blue;
                lbBottom.BackColor = Color.Blue;
            }
            else if (directions[counter] == 1)
            {
                lbLeft.BackColor = Color.Blue;
                lbRight.BackColor = Color.Yellow;
                lbBottom.BackColor = Color.Blue;
            }
            else
            {
                lbLeft.BackColor = Color.Blue;
                lbRight.BackColor = Color.Blue;
                lbBottom.BackColor = Color.Yellow;
            }
            counter++;
        }

        private void resetSteps()
        {
            ls = ListnerStates.LS_INACTIVE;
            steps.Clear();
            step = 0;
        }

        private void _recog_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string SpeechText = e.Result.Text.ToLower();
            string Direction;

            CommandType command = SpeechToCommand(SpeechText);
            //Microsoft.VisualBasic.Interaction.MsgBox(command);

            bool directionalCommand = ((command == CommandType.CT_LEFT) || (command == CommandType.CT_RIGHT) || (command == CommandType.CT_BOTTOM));
            //Maven.SpeakAsync(SpeechText);

            if (command == CommandType.CT_RESET)
            {
                resetSteps();
                Maven.SpeakAsync("I have reset the game.");
            }

            //If the recognizer is in inactive mode, it needs to be woken up
            if (ls == ListnerStates.LS_INACTIVE)
            {
                if ((command == CommandType.CT_DISPLAY) || (directionalCommand && (cbDisplayChecked == true)))
                {
                    resetSteps();
                    
                    ls = ListnerStates.LS_LISTENING;
                    Maven.SpeakAsync("Listening...");
                    if (directionalCommand)
                    {
                        steps.Add(SpeechText);
                        Maven.SpeakAsync(SpeechText);
                    }
                }
            }

            //If the recognizer is in listening mode, it will listen for top, left, right directional commands
            else if (ls == ListnerStates.LS_LISTENING)
            {
                if (directionalCommand)
                {
                    
                    if (command != lastCommand)
                    {
                        steps.Add(SpeechText);
                        Maven.SpeakAsync(SpeechText);
                    }

                    if ((cbScurryChecked == true) && (steps.Count == 6))
                    {
                        ls = ListnerStates.LS_SPEAKING;
                        step = 0;
                        Maven.SpeakAsync("Get Ready...");
                        Direction = "Go " + (string)steps[step];
                        Maven.SpeakAsync(Direction);
                    }
                }
                else if (command == CommandType.CT_SCURRY)
                {
                    
                    ls = ListnerStates.LS_SPEAKING;
                    step = 0;
                    Maven.SpeakAsync("Get Ready...");
                    Direction = "Go " + (string)steps[step];
                    Maven.SpeakAsync(Direction);
                    if (step == steps.Count - 1)
                    {
                        resetSteps();
                        Maven.SpeakAsync("Hope that went well!");
                        
                    }
                }
            }

            else if (ls == ListnerStates.LS_SPEAKING)
            {
                if (command == CommandType.CT_NEXT)
                {
                    
                    step++;
                    if (step < steps.Count)
                    {
                        Direction = "Go " + (string)steps[step];
                        Maven.SpeakAsync(Direction);
                        if (step == steps.Count - 1)
                        {
                            resetSteps();
                            Maven.SpeakAsync("Hope that went well!");
                            
                        }
                    }
                }
            }

            lastCommand = command;
        }
    }

    
}
