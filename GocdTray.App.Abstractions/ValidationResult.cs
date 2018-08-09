using System.Collections.Generic;
using System.Linq;

namespace GocdTray.App.Abstractions
{
    public class ValidationResult
    {
        public bool IsValid => !Messages.Any();
        public List<ValidationMessage> Messages { get; } = new List<ValidationMessage>();
        public void Add(string message, string property = null) => Messages.Add(new ValidationMessage {Message = message, Property = property});
        public override string ToString() => string.Join(" ", Messages.Select(m => m.Message));
    }

    public class ValidationMessage
    {
        public string Message { get; set; }
        public string Property { get; set; }
    }
}