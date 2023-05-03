using UnityEngine;
using SpeechLib;
using System.Threading.Tasks;
using TMPro;
using System;

public class TaxCalculator : MonoBehaviour
{
    // Constant rate for the Medicare Levy
    const double MEDICARE_LEVY = 0.02;

    // Variables
    bool textToSpeechEnabled = true;

    public TMP_InputField inputSalary;
    public TMP_Text netincome, grossSalary, incomeTax, medicarelevy;
    public TMP_Dropdown payperiod;


    private void Start()
    {
        Speak("Welcome to the A.T.O. Tax Calculator");
        //UnitTesting();
    }

    private void UnitTesting()
    {
        double[] testArray = { 10000, 18200, 30000, 45000, 70000, 120000, 150000, 180000, 200000 };
        double[] testAnswers = { 0, 0, 2242, 6172, 14297, 31897, 42997, 54097, 63097 };

        for (int i = 0; i < testArray.Length; i++)
        {
            if (CalculateIncomeTax(testArray[i]) == testAnswers[i])
            {
                print("Correct");
            }
            else
            {
                Debug.LogError("Incorrect");
                print("Salary: " + testArray[i]);
                print("Answer Given: " + CalculateIncomeTax(testArray[i]));
                print("Correct Answer: " + testAnswers[i]);
            }
            
        }
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
        double grossSalary;
        if (!double.TryParse(inputSalary.text, out grossSalary))
        {
            print("Error");
        }

        return grossSalary;

    }

    private int GetSalaryPayPeriod()
    {
        
        int salaryPayPeriod = payperiod.value;
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
        grossSalary.text = $"Gross Yearly Salary: {Math.Round(grossYearlySalary)}";
        double incomeTaxPaid = 0;
        if (grossYearlySalary <= 18200)
        {
            incomeTaxPaid = 0;
        }
        else if(grossYearlySalary <= 37000)
        {
            incomeTaxPaid = (grossYearlySalary - 18200) * 0.19f;
        }
        else if (grossYearlySalary <= 87000)
        {
            incomeTaxPaid = ((grossYearlySalary - 37000) * 0.325f) + 3572;
        }
        else if (grossYearlySalary <= 180000)
        {
            incomeTaxPaid = ((grossYearlySalary - 87000) * 0.37f) + 19822;
        }
        else
        {
            incomeTaxPaid = ((grossYearlySalary - 180000) * 0.45f) + 54232;
        }
        //print(incomeTaxPaid);
        
        return Math.Round(incomeTaxPaid);
    }

    private void OutputResults(double medicareLevyPaid, double incomeTaxPaid, double netIncome)
    {
        medicarelevy.text = $"Medicare Levy paid: {Math.Round(medicareLevyPaid).ToString()}" ;
        incomeTax.text = $"Income tax paid: {Math.Round(incomeTaxPaid).ToString()}";
        netincome.text = $"Net income: {Math.Round(netIncome).ToString()}";
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
