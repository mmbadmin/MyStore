namespace MyStore.Common.Application.Exceptions
{
    using FluentValidation.Results;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Failures = new Dictionary<string, string[]>();
            Messages = new List<string>();
        }

        public ValidationException(List<ValidationFailure> failures)
            : this()
        {
            var failureGroups = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage);

            foreach (var failureGroup in failureGroups)
            {
                var propertyName = failureGroup.Key;
                var propertyFailures = failureGroup.ToArray();
                Failures.Add(propertyName, propertyFailures);
                foreach (var propertyFailure in propertyFailures)
                {
                    Messages.Add(propertyFailure);
                }
            }
        }

        public IList<string> Messages { get; }

        public IDictionary<string, string[]> Failures { get; }
    }
}
