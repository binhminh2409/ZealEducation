namespace ZealEducation.Utils
{
    public class IdCreator
    {
        public IdCreator() { }

        //Create Id from a dynamic number of variables
        public string CreateId(params object[] values) 
        {
            string id = string.Join("_", values);

            return id;
        }
    }
}
