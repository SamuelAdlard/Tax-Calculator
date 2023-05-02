using UnityEngine;
using SpeechLib;
using System.Threading.Tasks;

public class TaxCalculator : MonoBehaviour
{
    // Constant rate for the Medicare Levy
    const double MEDICARE_LEVY = 0.02;

    // Variables
    bool textToSpeechEnabled = true;

    private void Start()
    {
        Speak("Welcome to the A.T.O. Tax Calculator");
    }

    // Run this function on the click event of your 'Calculate' button
    public void Calculate()
    {
        // Initialisation of variables
        double medicareLevyPaid = 0;
        double incomeTaxPaid = 0;

        // Input
        double grossSalaryInput = GetGrossSalary();
        int salaryPayPeriod = GetSalaryPayPeriod();

        // Calculations
        double grossYearlySalary = CalculateGrossYearlySalary(grossSalaryInput, salaryPayPeriod);
        double netIncome = CalculateNetIncome(grossYearlySalary, ref medicareLevyPaid, ref incomeTaxPaid);

        // Output
        OutputResults(medicareLevyPaid, incomeTaxPaid, netIncome);
    }

    private double GetGrossSalary()
    {
        // Get from user. E.g. input box
        // Validate the input (ensure it is a positive, valid number)
        double grossYearlySalary = 1000;
        return grossYearlySalary;
    }

    private int GetSalaryPayPeriod()
    {
        // Get from user. E.g. combobox or radio buttons
        int salaryPayPeriod = 0;
        return salaryPayPeriod;
    }

    private double CalculateGrossYearlySalary(double grossSalaryInput, int salaryPayPeriod)
    {
        double[] payPeriods = { 52.1429f, 26.0714f, 12, 1 };
        
        
        double grossYearlySalary = payPeriods[salaryPayPeriod] * grossSalaryInput;
        return grossYearlySalary;
    }

    private double CalculateNetIncome(double grossYearlySalary, ref double medicareLevyPaid, ref double incomeTaxPaid)
    {
        // This is a stub, replace with the real calculation and return the result
        medicareLevyPaid = CalculateMedicareLevy(grossYearlySalary);
        incomeTaxPaid = CalculateIncomeTax(grossYearlySalary);
        double netIncome = grossYearlySalary - medicareLevyPaid - incomeTaxPaid;        
        return netIncome;
    }

    private double CalculateMedicareLevy(double grossYearlySalary)
    {
        
        double medicareLevyPaid = grossYearlySalary * MEDICARE_LEVY;   
        

        return medicareLevyPaid;
    }

    private double CalculateIncomeTax(double grossYearlySalary)
    {
        
        double incomeTaxPaid = 15000;
        if (grossYearlySalary <= 18200)
        {
            incomeTaxPaid = 0;
        }
        else if(grossYearlySalary <= 45000)
        {
            incomeTaxPaid = (grossYearlySalary - 18200) * 0.19f;
        }
        else if (grossYearlySalary <= 120000)
        {
            incomeTaxPaid = ((grossYearlySalary - 45000) * 32.5f) + 5092;
        }
        else if (grossYearlySalary <= 180000)
        {
            incomeTaxPaid = ((grossYearlySalary - 120000) * 37f) + 29467;
        }
        else
        {
            incomeTaxPaid = ((grossYearlySalary - 180000) * 45) + 180000;
        }
        return incomeTaxPaid;
    }

    private void OutputResults(double medicareLevyPaid, double incomeTaxPaid, double netIncome)
    {
        // Output the following to the GUI
        // "Medicare levy paid: $" + medicareLevyPaid.ToString("F2");
        // "Income tax paid: $" + incomeTaxPaid.ToString("F2");
        // "Net income: $" + netIncome.ToString("F2");
    }

    // Text to Speech
    private async void Speak(string textToSpeech)
    {
        if(textToSpeechEnabled)
        {
            SpVoice voice = new SpVoice();
            await SpeakAsync(textToSpeech);
        }
    }

    private Task SpeakAsync(string textToSpeak)
    {
        return Task.Run(() =>
        {
            SpVoice voice = new SpVoice();
            voice.Speak(textToSpeak);
        });
    }
}
