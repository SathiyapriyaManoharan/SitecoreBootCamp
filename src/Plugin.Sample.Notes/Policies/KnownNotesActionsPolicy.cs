using Sitecore.Commerce.Core;

namespace Plugin.Bootcamp.Exercises.Notes
{
    public class KnownNotesActionsPolicy : Policy
    {
        public string EditNotes { get; set; } = nameof(EditNotes);
    }
}