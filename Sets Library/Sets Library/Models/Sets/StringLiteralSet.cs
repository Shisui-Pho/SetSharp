using SetsLibrary.Interfaces;

namespace SetsLibrary.Models
{
    public class StringLiteralSet : BaseSet<string>
    {
        #region Constructors
        public StringLiteralSet(SetExtractionConfiguration<string> extractionConfiguration) : base(extractionConfiguration)
        {
        }

        public StringLiteralSet(string expression, SetExtractionConfiguration<string> config) : base(expression, config)
        {
        }
        #endregion Constructors
        #region Ovverides
        protected override IStructuredSet<string> BuildNewSet(string setString)
        {
            return new StringLiteralSet(setString, this.ExtractionConfiguration);
        }//BuildNewSet

        protected override IStructuredSet<string> BuildNewSet()
        {
            return new StringLiteralSet(this.ExtractionConfiguration);
        }//BuildNewSet
        #endregion Ovverides
    }//class
}//namespace