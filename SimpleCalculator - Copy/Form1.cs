using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleCalculator
{
    public partial class Calculator : Form
    {
        private bool isOperationPerformed = false;
        private double result = 0;
        private string operationalSign = string.Empty;
        private Timer animationTimer;
        private int animationCounter = 0;

        public Calculator()
        {
            InitializeComponent();
            InitializeAnimationTimer();
        }

        private void InitializeAnimationTimer()
        {
            animationTimer = new Timer();
            animationTimer.Interval = 500; // 500 milliseconds = 0.5 seconds
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (animationCounter % 2 == 0)
                textBox_result1.ForeColor = Color.Red; // Set text color to red on even animation counter
            else
                textBox_result1.ForeColor = Color.Black; // Set text color to black on odd animation counter

            animationCounter++;

            if (animationCounter >= 6)
            {
                animationCounter = 0;
                animationTimer.Stop(); // Stop the timer after 3 color changes
                textBox_result1.ForeColor = Color.Black; // Revert text color back to black
            }
        }

        private void Click(object sender, EventArgs e)
        {
            if (textBox_result1.Text == "0" || isOperationPerformed)
                textBox_result1.Clear();

            isOperationPerformed = false;

            Button button = (Button)sender;
            if (button.Text == ".")
            {
                if (!textBox_result1.Text.Contains("."))
                {
                    textBox_result1.Text += button.Text;
                }
            }
            else
            {
                textBox_result1.Text += button.Text;
            }
        }

        private void Operational_button(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            // Check if an operation is already in progress
            if (isOperationPerformed)
            {
                // Update the current operation sign
                operationalSign = button.Text;
                currentDisplay.Text = currentDisplay.Text.Remove(currentDisplay.Text.Length - 1) + operationalSign;
            }
            else
            {
                // If no operation in progress, start a new one
                if (double.TryParse(textBox_result1.Text, out double value))
                {
                    CalculateResult(value);
                    operationalSign = button.Text;
                    currentDisplay.Text = result.ToString("#,0") + " " + operationalSign; // Add comma separators to the result
                }
                else
                {
                    // Handle invalid input
                    MessageBox.Show("Invalid input. Please enter a valid number.");
                }
            }
            // Reset the flag for the next operation
            isOperationPerformed = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (double.TryParse(textBox_result1.Text, out double value))
            {
                CalculateResult(value);
                currentDisplay.Text = "";
            }
            else
            {
                // Handle invalid input
                MessageBox.Show("Invalid input. Please enter a valid number.");
            }
        }

        private void CalculateResult(double value)
        {
            switch (operationalSign)
            {
                case "+":
                    result += value;
                    break;
                case "-":
                    result -= value;
                    break;
                case "*":
                    result *= value;
                    break;
                case "/":
                    if (value != 0)
                        result /= value;
                    else
                    {
                        // Handle division by zero
                        textBox_result1.ForeColor = Color.Red;
                        textBox_result1.Text = "Cannot divide by 0.";
                        animationCounter = 0; // Reset the animation counter
                        animationTimer.Start(); // Start the animation timer
                        return;
                    }
                    break;
                case "":
                    result = value;
                    break;
            }
            textBox_result1.Text = result.ToString("#,0"); // Add comma separators to the result
            operationalSign = "";
            isOperationPerformed = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox_result1.Text.Length > 0)
                textBox_result1.Text = textBox_result1.Text.Substring(0, textBox_result1.Text.Length - 1);

            if (textBox_result1.Text.Length <= 0)
            {
                textBox_result1.Text = "0";
                result = 0;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox_result1.Text = "0";
            currentDisplay.Text = "";
            result = 0;
            operationalSign = "";
            isOperationPerformed = false;
        }
    }
}
