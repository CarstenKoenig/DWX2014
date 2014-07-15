namespace Monaden.CSharp
{
    public class Unit 
    {
        static readonly Unit _value = new Unit();

        private  Unit ()
        {
        }

        public static Unit V
        {
            get { return _value; }
        }
    }
}