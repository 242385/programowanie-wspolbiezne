namespace Dane
{
    public abstract class AbstractDataAPI
    { 
       public static AbstractDataAPI CreateNewInstance()
       {
           return new DataAPI();
       }

       internal sealed class DataAPI : AbstractDataAPI
       {

       }
    }
}