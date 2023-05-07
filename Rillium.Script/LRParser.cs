namespace Rillium.Script
{
    public class LRParser
    {
        private List<Tuple<int, string, int>> parsingTable;
        private Stack<int> stateStack;
        private Stack<string> symbolStack;

        public LRParser()
        {
            parsingTable = new List<Tuple<int, string, int>>();
            stateStack = new Stack<int>();
            symbolStack = new Stack<string>();
        }

        public void AddParsingRule(int state, string symbol, int action, bool shift)
        {
            parsingTable.Add(new Tuple<int, string, int>(state, symbol, action));

            if (shift)
            {
                // Add the new state to the stack if it's a shift action
                stateStack.Push(action);
            }
        }

        public bool Parse(string input)
        {
            var pos = 0;

            while (true)
            {
                var state = stateStack.Peek();
                var symbol = input[pos].ToString();

                var action = GetParsingAction(state, symbol);

                if (action == -1)
                {
                    // Parsing failed, symbol not recognized
                    return false;
                }

                if (action == 0)
                {
                    // Parsing succeeded, accept the input
                    return true;
                }

                if (action > 0)
                {
                    // Shift the symbol onto the stack
                    stateStack.Push(action);
                    symbolStack.Push(symbol);
                    pos++;
                }
                else
                {
                    // Reduce the symbols on the stack using the parsing rule
                    var rule = -action;

                    for (var i = 0; i < GetRuleLength(rule); i++)
                    {
                        stateStack.Pop();
                        symbolStack.Pop();
                    }

                    state = stateStack.Peek();
                    symbol = GetRuleSymbol(rule);

                    var newState = GetParsingAction(state, symbol);

                    // Push the new state onto the stack
                    stateStack.Push(newState);
                    symbolStack.Push(symbol);
                }
            }
        }

        private int GetParsingAction(int state, string symbol)
        {
            foreach (var rule in parsingTable)
            {
                if (rule.Item1 == state && rule.Item2 == symbol)
                {
                    return rule.Item3;
                }
            }

            return -1;
        }

        private int GetRuleLength(int rule)
        {
            // Return the length of the right-hand side of the parsing rule
            return 1;
        }

        private string GetRuleSymbol(int rule)
        {
            // Return the left-hand side of the parsing rule
            return "S";
        }
    }
}