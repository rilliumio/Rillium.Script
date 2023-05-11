namespace Rillium.Script
{
    internal class Rules
    {
        public static void SetRules(LRParser parser)
        {
            // Add the parsing rules for the grammar
            parser.AddParsingRule(0, "expr", 1, true);
            parser.AddParsingRule(1, "+", 2, true);
            parser.AddParsingRule(1, "$", 0, false);
            parser.AddParsingRule(2, "term", 3, true);
            parser.AddParsingRule(3, "*", 4, true);
            parser.AddParsingRule(3, "+", -2, false);
            parser.AddParsingRule(3, "$", -2, false);
            parser.AddParsingRule(4, "factor", 5, true);
            parser.AddParsingRule(5, "+", -3, false);
            parser.AddParsingRule(5, "*", -3, false);
            parser.AddParsingRule(5, "$", -3, false);
        }
    }
}
