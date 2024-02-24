using System.Text;

namespace ContosoUniversity
{
    public class TestConvertToString<T>:List<T>
    {
        public string TypeName { get; set; }
        public int ListCount { get; set; }



        public TestConvertToString(List<T> items)
        {
            TypeName=typeof(T).Name;
            ListCount=items.Count;

            this.AddRange(items);
        }

        public static async Task<string> ConvertString(IQueryable<T> source) 
        {
            StringBuilder ResultMessage = new StringBuilder();
            foreach (T item in source)
            {
                var properties=item.GetType().GetProperties();
                var properMessage = new StringBuilder();
                foreach (var property in properties)
                {
                    properMessage.AppendFormat(@"{0}:""{1}"",",property.Name,property.GetValue(item,null));
                }
                ResultMessage.AppendFormat("[{0}]", properMessage?.ToString());
            }

            return ResultMessage.ToString();
        }
    }
}
