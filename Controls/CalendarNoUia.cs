using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace PolMedUMG.Controls
{
    /// <summary>
    /// Kalendarz bez obsługi UI-Automation 
    /// zapobiega crashowania aplikacji po zmianie ustawienia
    /// kontrolki w "umów wizytę" i przełączeniu na kalendarz
    /// błąd prawdopodobnie związany z istniejącym zgłoszeniem https://github.com/dotnet/wpf/issues/8188
    /// </summary>
    public class CalendarNoUia : Calendar
    {
        protected override AutomationPeer OnCreateAutomationPeer() => null;
    }
}