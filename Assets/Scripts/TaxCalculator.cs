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
    public TMP_Text NetIncomeText, GrossSalaryText, IncomeTaxText, MedicareLevyText, ConvertButtonText, Placeholder, GrossSalary;
    public TMP_Dropdown payperiod;
    public TMP_Dropdown Language_dropdown;
    public string Language = "English";


    double medicareLevyPaid = 0;
    double incomeTaxPaid = 0;
    double grossSalaryInput;
    int salaryPayPeriod;
    double grossYearlySalary;
    double netIncome;
    private void Start()
    {
        Speak("Welcome to the A.T.O. Tax Calculator");
        //UnitTesting();
    }

    private void UnitTesting()
    {   //ignore this was testing-sam
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
        

        // Input
        grossSalaryInput = GetGrossSalary();
        salaryPayPeriod = GetSalaryPayPeriod();

        // Calculations
        grossYearlySalary = CalculateGrossYearlySalary(grossSalaryInput, salaryPayPeriod);
        netIncome = CalculateNetIncome(grossYearlySalary, ref medicareLevyPaid, ref incomeTaxPaid);

        // Output
        OutputResults(medicareLevyPaid, incomeTaxPaid, netIncome);
    }

    private double GetGrossSalary()
    {
        double grossSalary;
        if (!double.TryParse(inputSalary.text, out grossSalary))
        {
            //stops the program from crashing when an invalid character is put in
            print("Error");
        }

        return grossSalary;

    }

    private int GetSalaryPayPeriod()
    {
        //salary input
        int salaryPayPeriod = payperiod.value;
        return salaryPayPeriod;
    }

    private double CalculateGrossYearlySalary(double grossSalaryInput, int salaryPayPeriod)
    {
        //list of payperiods in numbers
        double[] payPeriods = { 52.1429f, 26.0714f, 12, 1 };
        
        //identifys pay periods
        double grossYearlySalary = payPeriods[salaryPayPeriod] * grossSalaryInput;
        return grossYearlySalary;
    }

    private double CalculateNetIncome(double grossYearlySalary, ref double medicareLevyPaid, ref double incomeTaxPaid)
    {
        // calculations for the text boxes
        medicareLevyPaid = CalculateMedicareLevy(grossYearlySalary);
        incomeTaxPaid = CalculateIncomeTax(grossYearlySalary);
        double netIncome = grossYearlySalary - medicareLevyPaid - incomeTaxPaid;        
        return netIncome;
    }
    //medicare levy from from gross salary
    private double CalculateMedicareLevy(double grossYearlySalary)
    {
        
        double medicareLevyPaid = grossYearlySalary * MEDICARE_LEVY;   
        

        return medicareLevyPaid;
    }

    private double CalculateIncomeTax(double grossYearlySalary)
    {
        GrossSalaryText.text = $"Gross Yearly Salary: {Math.Round(grossYearlySalary)}";
        double incomeTaxPaid = 0;
        // finds tax bracket and files the tax and initail payment
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
        // translation texts
        if (Language == "Norsk")
        {
            MedicareLevyText.text = $"Medicare avgift betalt: {Math.Round(medicareLevyPaid)}";
            IncomeTaxText.text = $"Innbetalt inntektsskatt: {Math.Round(incomeTaxPaid)}";
            NetIncomeText.text = $"Netto inntekt: {Math.Round(netIncome)}";
            ConvertButtonText.text = "Konvertere";
            Placeholder.text = "skriv inn lønn";
            GrossSalary.text = "brutto lønn";
        }
        else if(Language == "English")
        {
            MedicareLevyText.text = $"Medicare Levy paid: {Math.Round(medicareLevyPaid).ToString()}";
            IncomeTaxText.text = $"Income tax paid: {Math.Round(incomeTaxPaid).ToString()}";
            NetIncomeText.text = $"Net income: {Math.Round(netIncome).ToString()}";
            ConvertButtonText.text = "Convert";
            Placeholder.text = "Enter Salary";
            GrossSalary.text = "gross salary";
        }
        else
        {
            MedicareLevyText.text = $"plunder for healys: {Math.Round(medicareLevyPaid).ToString()}";
            IncomeTaxText.text = $"Treasure stolen: {Math.Round(incomeTaxPaid).ToString()}";
            NetIncomeText.text = $"Yer plunder: {Math.Round(netIncome).ToString()}";
            ConvertButtonText.text = "MATHY";
            Placeholder.text = "Shovel Dubloons";
            GrossSalary.text = "YER ANual PLunder";
        }
       
        // "Medicare levy paid: $" + medicareLevyPaid.ToString("F2");
        // "Income tax paid: $" + incomeTaxPaid.ToString("F2");
        // "Net income: $" + netIncome.ToString("F2");
        // "convert: $" + convertbutton.ToString("F2");
    }

    public void LanguageChange()
    {
        //detects the languauge selected in the dropdown and changes the language variable to text displayed
        Language = Language_dropdown.captionText.text;
        print(Language_dropdown.captionText.text);
        OutputResults(medicareLevyPaid, incomeTaxPaid, netIncome);
    }


        // Text to Speech
    public async void Speak(string textToSpeech)
    {
        if(textToSpeechEnabled)
        {
            SpVoice voice = new SpVoice();
            await SpeakAsync(textToSpeech);
        }
    }
    //Text to speak
    private Task SpeakAsync(string textToSpeak)
    {
        return Task.Run(() =>
        {
            SpVoice voice = new SpVoice();
            _ = voice.Speak(textToSpeak);
        });
    }
}
