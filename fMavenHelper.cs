using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

//System.Speech.dll references -->
using System.Speech.Recognition;
using System.Speech.Synthesis;


namespace MavenHelper
{    
    public partial class fMavenHelper : Form
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
        int inactiveValue;

        ArrayList steps = new ArrayList();
        int step, inactivityCounter = 0;
        CommandType lastCommand = CommandType.CT_INVALID;

        public fMavenHelper()
        {
            InitializeComponent();
        }

        private void fMavenHelper_Load(object sender, EventArgs e)
        {
            FileStream commandsFile = File.Open("Commands.dat", FileMode.Open);
            BinaryFormatter reader = new BinaryFormatter();
            left = (ArrayList) reader.Deserialize(commandsFile);
            right = (ArrayList) reader.Deserialize(commandsFile);
            bottom = (ArrayList) reader.Deserialize(commandsFile);
            display = (ArrayList)reader.Deserialize(commandsFile);
            scurry = (ArrayList)reader.Deserialize(commandsFile);
            nextprompt = (ArrayList)reader.Deserialize(commandsFile);
            resetCommands = (ArrayList)reader.Deserialize(commandsFile);
            cbDisplay.Checked = (bool)reader.Deserialize(commandsFile);
            cbScurry.Checked = (bool)reader.Deserialize(commandsFile);
            inactiveValue = (int)reader.Deserialize(commandsFile);
            commandsFile.Close();

            foreach (string item in left)
                lbLeft.Items.Add(item);
            foreach (string item in right)
                lbRight.Items.Add(item);
            foreach (string item in bottom)
                lbBottom.Items.Add(item);
            foreach (string item in display)
                lbDisplay.Items.Add(item);
            foreach (string item in scurry)
                lbScurry.Items.Add(item);
            foreach (string item in nextprompt)
                lbNext.Items.Add(item);

            tbInactive.Text = inactiveValue.ToString();

            _recog.SetInputToDefaultAudioDevice();
            RefreshCommands();
            _recog.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(_recog_SpeechRecognized);
            Maven.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
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
            foreach (string item in resetCommands)
                grammarRules.Add(item);

            grammar = (string[])grammarRules.ToArray(typeof(string));
            _recog.UnloadAllGrammars();
            _recog.LoadGrammar(new Grammar(new GrammarBuilder(new Choices(grammar))));
        }

        private void RefreshFile ()
        {
            FileStream commandsFile = File.Create("Commands.dat");
            BinaryFormatter writer = new BinaryFormatter();
            writer.Serialize(commandsFile, left);
            writer.Serialize(commandsFile, right);
            writer.Serialize(commandsFile, bottom);
            writer.Serialize(commandsFile, display);
            writer.Serialize(commandsFile, scurry);
            writer.Serialize(commandsFile, nextprompt);

            resetCommands.Clear();
            resetCommands.Add("stop");
            resetCommands.Add("reset");
            writer.Serialize(commandsFile, resetCommands);

            writer.Serialize(commandsFile, cbDisplay.Checked);
            writer.Serialize(commandsFile, cbScurry.Checked);
            writer.Serialize(commandsFile, inactiveValue);

            
            commandsFile.Close();
        }

        private bool isStringValid (string command)
        {
            return Regex.IsMatch(command, @"^[a-zA-Z]+$");
        }

        private void btLeftAdd_Click(object sender, EventArgs e)
        {
            string newCommand = Microsoft.VisualBasic.Interaction.InputBox("Enter new Command:", "Maven Helper", "", -1, -1);
            if ((newCommand != "") && (isStringValid(newCommand)))
            {                
                newCommand = newCommand.ToLower();
                if (left.IndexOf(newCommand) >= 0)
                    Microsoft.VisualBasic.Interaction.MsgBox("Command " + newCommand + " is already present for this zone. Please try another command.", Microsoft.VisualBasic.MsgBoxStyle.Information, "Maven Helper");
                else if ((bottom.IndexOf(newCommand) >= 0) || (right.IndexOf(newCommand) >= 0) || (display.IndexOf(newCommand) >= 0) || (scurry.IndexOf(newCommand) >= 0) || (nextprompt.IndexOf(newCommand) >= 0))
                    Microsoft.VisualBasic.Interaction.MsgBox("Command " + newCommand + " is already present for another zone. Please try another command.", Microsoft.VisualBasic.MsgBoxStyle.Information, "Maven Helper");
                else
                {
                    left.Add(newCommand);
                    lbLeft.Items.Add(newCommand);
                    RefreshFile();
                    RefreshCommands();
                }
            }
            else
                Microsoft.VisualBasic.Interaction.MsgBox("Invalid Command! Valid commands must only contain letters from a-z", Microsoft.VisualBasic.MsgBoxStyle.OkOnly, "Maven Helper");
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

            if(command == CommandType.CT_RESET)
            {
                resetSteps();
                Maven.SpeakAsync("I have reset the game.");
                inactivityCounter = 0;
                tInactivity.Stop();
            }


            //If the recognizer is in inactive mode, it needs to be woken up
            if (ls == ListnerStates.LS_INACTIVE) {
                if ((command == CommandType.CT_DISPLAY) || (directionalCommand && (cbDisplay.Checked == true)))
                {
                    resetSteps();
                    tInactivity.Start();
                    inactivityCounter = 0;
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
                    inactivityCounter = 0;
                    if (command != lastCommand)
                    {
                        steps.Add(SpeechText);
                        Maven.SpeakAsync(SpeechText);
                    }

                    if ((cbScurry.Checked == true) && (steps.Count == 6))
                    {
                        ls = ListnerStates.LS_SPEAKING;
                        step = 0;
                        Maven.SpeakAsync("Get Ready...");
                        Direction = "Go " + (string)steps[step];
                        Maven.SpeakAsync(Direction);
                    }
                }
                else if(command == CommandType.CT_SCURRY)
                {
                    inactivityCounter = 0;
                    ls = ListnerStates.LS_SPEAKING;
                    step = 0;
                    Maven.SpeakAsync("Get Ready...");
                    Direction = "Go " + (string)steps[step];
                    Maven.SpeakAsync(Direction);
                    if (step == steps.Count - 1)
                    {
                        resetSteps();
                        Maven.SpeakAsync("Hope that went well!");
                        tInactivity.Stop();
                    }
                }
            }
                
            else if (ls == ListnerStates.LS_SPEAKING)
            {
                if(command == CommandType.CT_NEXT)
                {
                    inactivityCounter = 0;
                    step++;
                    if (step < steps.Count)
                    {
                        Direction = "Go " + (string)steps[step];
                        Maven.SpeakAsync(Direction);
                        if (step == steps.Count - 1)
                        {
                            resetSteps();
                            Maven.SpeakAsync("Hope that went well!");
                            tInactivity.Stop();
                        }
                    }
                }
            }

            lastCommand = command;
        }

        private void _recog_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void btLeftRemove_Click(object sender, EventArgs e)
        {
            string selectedCommand = (string)lbLeft.SelectedItem;
            if (selectedCommand != "")
            {
                left.Remove(selectedCommand);
                left.TrimToSize();
                lbLeft.Items.Remove(selectedCommand);
                RefreshFile();
                RefreshCommands();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btRightAdd_Click(object sender, EventArgs e)
        {
            string newCommand = Microsoft.VisualBasic.Interaction.InputBox("Enter new Command:", "Maven Helper", "", -1, -1);
            if ((newCommand != "") && (isStringValid(newCommand)))
            {
                newCommand = newCommand.ToLower();
                if (right.IndexOf(newCommand) >= 0)
                    Microsoft.VisualBasic.Interaction.MsgBox("Command " + newCommand + " is already present for this zone. Please try another command.", Microsoft.VisualBasic.MsgBoxStyle.Information, "Maven Helper");
                else if ((left.IndexOf(newCommand) >= 0) || (bottom.IndexOf(newCommand) >= 0) || (display.IndexOf(newCommand) >= 0) || (scurry.IndexOf(newCommand) >= 0) || (nextprompt.IndexOf(newCommand) >= 0))
                    Microsoft.VisualBasic.Interaction.MsgBox("Command " + newCommand + " is already present for another zone. Please try another command.", Microsoft.VisualBasic.MsgBoxStyle.Information, "Maven Helper");
                else
                {
                    right.Add(newCommand);
                    lbRight.Items.Add(newCommand);
                    RefreshFile();
                    RefreshCommands();
                }
            }
            else
                Microsoft.VisualBasic.Interaction.MsgBox("Invalid Command! Valid commands must only contain letters from a-z", Microsoft.VisualBasic.MsgBoxStyle.OkOnly, "Maven Helper");
        }

        private void btBottomAdd_Click(object sender, EventArgs e)
        {
            string newCommand = Microsoft.VisualBasic.Interaction.InputBox("Enter new Command:", "Maven Helper", "", -1, -1);
            if ((newCommand != "") && (isStringValid(newCommand)))
            {
                newCommand = newCommand.ToLower();
                if (bottom.IndexOf(newCommand) >= 0)
                    Microsoft.VisualBasic.Interaction.MsgBox("Command " + newCommand + " is already present for this zone. Please try another command.", Microsoft.VisualBasic.MsgBoxStyle.Information, "Maven Helper");
                else if ((left.IndexOf(newCommand) >= 0) || (right.IndexOf(newCommand) >= 0) || (display.IndexOf(newCommand) >= 0) || (scurry.IndexOf(newCommand) >= 0) || (nextprompt.IndexOf(newCommand) >= 0))
                    Microsoft.VisualBasic.Interaction.MsgBox("Command " + newCommand + " is already present for another zone. Please try another command.", Microsoft.VisualBasic.MsgBoxStyle.Information, "Maven Helper");
                else
                {
                    bottom.Add(newCommand);
                    lbBottom.Items.Add(newCommand);
                    RefreshFile();
                    RefreshCommands();
                }
            }
            else
                Microsoft.VisualBasic.Interaction.MsgBox("Invalid Command! Valid commands must only contain letters from a-z", Microsoft.VisualBasic.MsgBoxStyle.OkOnly, "Maven Helper");
        }

        private void btDisplayAdd_Click(object sender, EventArgs e)
        {
            string newCommand = Microsoft.VisualBasic.Interaction.InputBox("Enter new Command:", "Maven Helper", "", -1, -1);
            if ((newCommand != "") && (isStringValid(newCommand)))
            {
                newCommand = newCommand.ToLower();
                if (display.IndexOf(newCommand) >= 0)
                    Microsoft.VisualBasic.Interaction.MsgBox("Command " + newCommand + " is already present for this zone. Please try another command.", Microsoft.VisualBasic.MsgBoxStyle.Information, "Maven Helper");
                else if ((left.IndexOf(newCommand) >= 0) || (bottom.IndexOf(newCommand) >= 0) || (right.IndexOf(newCommand) >= 0) || (scurry.IndexOf(newCommand) >= 0) || (nextprompt.IndexOf(newCommand) >= 0))
                    Microsoft.VisualBasic.Interaction.MsgBox("Command " + newCommand + " is already present for another zone. Please try another command.", Microsoft.VisualBasic.MsgBoxStyle.Information, "Maven Helper");
                else
                {
                    display.Add(newCommand);
                    lbDisplay.Items.Add(newCommand);
                    RefreshFile();
                    RefreshCommands();
                }
            }
            else
                Microsoft.VisualBasic.Interaction.MsgBox("Invalid Command! Valid commands must only contain letters from a-z", Microsoft.VisualBasic.MsgBoxStyle.OkOnly, "Maven Helper");
        }

        private void btScurryAdd_Click(object sender, EventArgs e)
        {
            string newCommand = Microsoft.VisualBasic.Interaction.InputBox("Enter new Command:", "Maven Helper", "", -1, -1);
            if ((newCommand != "") && (isStringValid(newCommand)))
            {
                newCommand = newCommand.ToLower();
                if (scurry.IndexOf(newCommand) >= 0)
                    Microsoft.VisualBasic.Interaction.MsgBox("Command " + newCommand + " is already present for this zone. Please try another command.", Microsoft.VisualBasic.MsgBoxStyle.Information, "Maven Helper");
                else if ((left.IndexOf(newCommand) >= 0) || (bottom.IndexOf(newCommand) >= 0) || (display.IndexOf(newCommand) >= 0) || (right.IndexOf(newCommand) >= 0) || (nextprompt.IndexOf(newCommand) >= 0))
                    Microsoft.VisualBasic.Interaction.MsgBox("Command " + newCommand + " is already present for another zone. Please try another command.", Microsoft.VisualBasic.MsgBoxStyle.Information, "Maven Helper");
                else
                {
                    scurry.Add(newCommand);
                    lbScurry.Items.Add(newCommand);
                    RefreshFile();
                    RefreshCommands();
                }
            }
            else
                Microsoft.VisualBasic.Interaction.MsgBox("Invalid Command! Valid commands must only contain letters from a-z", Microsoft.VisualBasic.MsgBoxStyle.OkOnly, "Maven Helper");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string newCommand = Microsoft.VisualBasic.Interaction.InputBox("Enter new Command:", "Maven Helper", "", -1, -1);
            if ((newCommand != "") && (isStringValid(newCommand)))
            {
                newCommand = newCommand.ToLower();
                if (nextprompt.IndexOf(newCommand) >= 0)
                    Microsoft.VisualBasic.Interaction.MsgBox("Command " + newCommand + " is already present for this zone. Please try another command.", Microsoft.VisualBasic.MsgBoxStyle.Information, "Maven Helper");
                else if ((left.IndexOf(newCommand) >= 0) || (bottom.IndexOf(newCommand) >= 0) || (display.IndexOf(newCommand) >= 0) || (scurry.IndexOf(newCommand) >= 0) || (right.IndexOf(newCommand) >= 0))
                    Microsoft.VisualBasic.Interaction.MsgBox("Command " + newCommand + " is already present for another zone. Please try another command.", Microsoft.VisualBasic.MsgBoxStyle.Information, "Maven Helper");
                else
                {
                    nextprompt.Add(newCommand);
                    lbNext.Items.Add(newCommand);
                    RefreshFile();
                    RefreshCommands();
                }
            }
            else
                Microsoft.VisualBasic.Interaction.MsgBox("Invalid Command! Valid commands must only contain letters from a-z", Microsoft.VisualBasic.MsgBoxStyle.OkOnly, "Maven Helper");
        }

        private void btBottomRemove_Click(object sender, EventArgs e)
        {
            string selectedCommand = (string)lbBottom.SelectedItem;
            if (selectedCommand != "")
            {
                bottom.Remove(selectedCommand);
                bottom.TrimToSize();
                lbBottom.Items.Remove(selectedCommand);
                RefreshFile();
                RefreshCommands();
            }
        }

        private void btRightRemove_Click(object sender, EventArgs e)
        {
            string selectedCommand = (string)lbRight.SelectedItem;
            if (selectedCommand != "")
            {
                right.Remove(selectedCommand);
                right.TrimToSize();
                lbRight.Items.Remove(selectedCommand);
                RefreshFile();
                RefreshCommands();
            }
        }

        private void btDisplayRemove_Click(object sender, EventArgs e)
        {
            string selectedCommand = (string)lbDisplay.SelectedItem;
            if (selectedCommand != "")
            {
                display.Remove(selectedCommand);
                display.TrimToSize();
                lbDisplay.Items.Remove(selectedCommand);
                RefreshFile();
                RefreshCommands();
            }
        }

        private void btScurryRemove_Click(object sender, EventArgs e)
        {
            string selectedCommand = (string)lbScurry.SelectedItem;
            if (selectedCommand != "")
            {
                scurry.Remove(selectedCommand);
                scurry.TrimToSize();
                lbScurry.Items.Remove(selectedCommand);
                RefreshFile();
                RefreshCommands();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string selectedCommand = (string)lbNext.SelectedItem;
            if (selectedCommand != "")
            {
                nextprompt.Remove(selectedCommand);
                nextprompt.TrimToSize();
                lbNext.Items.Remove(selectedCommand);
                RefreshFile();
                RefreshCommands();
            }
        }

        private void rbOFF_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOFF.Checked == true)
            {
                gbCommandEditor.Enabled = true;
                _recog.RecognizeAsyncStop();
                ls = ListnerStates.LS_INACTIVE;
                steps.Clear();
                step = 0;
                lbAlert.Visible = true;
            }
        }

        private void rbON_CheckedChanged(object sender, EventArgs e)
        {
            if(rbON.Checked == true)
            {
                gbCommandEditor.Enabled = false;                
                _recog.RecognizeAsync(RecognizeMode.Multiple);
                ls = ListnerStates.LS_INACTIVE;
                steps.Clear();
                step = 0;
                RefreshFile();
                lbAlert.Visible = false;
            }
        }

        private void cbDisplay_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btPractice_Click(object sender, EventArgs e)
        {
            gbCommandEditor.Enabled = true;
            _recog.RecognizeAsyncStop();
            ls = ListnerStates.LS_INACTIVE;
            steps.Clear();
            step = 0;
            rbOFF.Checked = true;
            Maven_Helper_Practice mhp = new Maven_Helper_Practice();
            mhp.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btDecrease_Click(object sender, EventArgs e)
        {
            int val = Int32.Parse(tbInactive.Text);
            if (val > 10)
                val--;
            tbInactive.Text = val.ToString();
            inactiveValue = val;
            RefreshFile();
        }

        private void btIncrease_Click(object sender, EventArgs e)
        {
            int val = Int32.Parse(tbInactive.Text);
            if (val < 60)
                val++;
            tbInactive.Text = val.ToString();
            inactiveValue = val;
            RefreshFile();
        }

        private void tInactivity_Tick(object sender, EventArgs e)
        {
            inactivityCounter++;
            if(inactivityCounter == inactiveValue)
            {
                resetSteps();
                Maven.SpeakAsync("I have reset the game.");
                inactivityCounter = 0;
                tInactivity.Stop();
            }
        }
    }
}
