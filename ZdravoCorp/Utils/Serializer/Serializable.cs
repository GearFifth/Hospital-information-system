namespace ZdravoCorp.Utils.Serializer
{
    public interface Serializable
    {
        string[] ToCSV();

        void FromCSV(string[] values);
    }
}
