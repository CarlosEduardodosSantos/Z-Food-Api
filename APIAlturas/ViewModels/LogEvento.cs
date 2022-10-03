using System;

namespace APIAlturas.ViewModels
{
    public class LogEvento
    {
        public LogEvento(string logLevel,string message)
        {
            Id = Guid.NewGuid();
            CreatedTime = DateTime.Now;
            LogLevel = logLevel;
            Message = message;

        }
        public Guid Id { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}