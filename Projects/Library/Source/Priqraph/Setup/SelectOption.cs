namespace Priqraph.Setup
{
    public class SelectOption
    {
        public int DefaultPageSize
        {
            get;
            set;
        } = 10;

        public LoadingRelatedObject LoadingRelatedObject
        {
            get;
            set;
        } = LoadingRelatedObject.Eager;

        public int QueryDepth
        {
            get;
            set;
        } = 2; //use Force tag to join more than Value of QueryDepth objects

        public IEnumerable<UserDefinedTable>? UserDefinedTables
        {
            get;
            set;
        }
    }
}