﻿namespace SparqlForHumans.Models
{
    public class Subject : ISubject
    {
        public Subject() { }

        public Subject(string id)
        {
            Id = id;
        }

        public Subject(string id, string label)
        {
            Id = id;
            Label = label;
        }

        public Subject(ISubject baseSubject)
        {
            Id = baseSubject.Id;
            Label = baseSubject.Label;
        }

        public string Id { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Label} ({Id})";
        }
    }
}