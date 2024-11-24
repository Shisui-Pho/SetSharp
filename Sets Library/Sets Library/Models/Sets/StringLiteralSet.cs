using SetsLibrary.Interfaces;

namespace SetsLibrary.Models
{
    public class StringLiteralSet : BaseSet<string>
    {
        public override bool Contains(string Element)
        {
            throw new NotImplementedException();
        }

        public override IStructuredSet<string> MergeWith(IStructuredSet<string> set)
        {
            throw new NotImplementedException();
        }
    }//class
}//namespace