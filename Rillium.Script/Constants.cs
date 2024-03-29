﻿namespace Rillium.Script
{
    internal class Constants
    {
        public const string OutputValueKey = "__return__";
        public const string EntryArgumentParameterName = "args";

        public class ExceptionMessages
        {
            public const string NameDoesNotExist = "The name '{0}' does not exist in the current context.";
            public const string UnassignedLocalVariable = "Use of unassigned local variable '{0}'.";
            public const string UnterminatedBlockComment = "End-of-file found, '*/' expected.";
        }
    }
}
