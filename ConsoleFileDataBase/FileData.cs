using System.Reflection;
using System.Text;

namespace ConsoleFileDataBase;

public class FileData {
    public string? FileParentDirectoryName;
    public string FileName;
    public long FileWeight;
    public string FileExtension;
    public DateTime FileCreationDate;
    public DateTime FileLastModifiedDate;

    public string ToStringPropertiesValueWithReflection() {
        StringBuilder sb = new();

        Type type = typeof(FileData);
        PropertyInfo[] properties = type.GetProperties();
        foreach (PropertyInfo propertyInfo in properties) {

            Type propertyType = propertyInfo.GetType();
            object? propertyValue = propertyInfo.GetValue(this);

            if (propertyValue is string) {
                string value = (string)propertyValue;
                sb.Append(value);
            }
            if (propertyValue is DateTime) {
                DateTime value = (DateTime)propertyValue;
                sb.Append(value.ToString());
            }
            if (propertyValue is long) {
                long value = (long)propertyValue;
                sb.Append(value.ToString());
            }

            sb.Append('\t');
        }

        return sb.ToString();
    }

    public static string ToStringPropertiesNameWithReflection() {
        StringBuilder sb = new();

        Type type = typeof(FileData);
        PropertyInfo[] properties = type.GetProperties();
        foreach (PropertyInfo propertyInfo in properties) {
            string propertyName = propertyInfo.Name;
            sb.Append(propertyName);
            sb.Append('\t');
        }

        return sb.ToString();
    }
}