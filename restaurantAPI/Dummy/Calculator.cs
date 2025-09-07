namespace restaurantAPI.Dummy
{
    public class Calculator
    {
        public int a { get; set; }
        public int b { get; set; }
        public string result { get; set; } = "";
        public int Add()
        {
            result= $"Addition performed {a} + {b} = {a+b}";
            return a + b;
        }

    }
}
