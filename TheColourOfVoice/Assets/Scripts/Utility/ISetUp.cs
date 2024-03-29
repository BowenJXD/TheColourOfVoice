/// <summary>
/// Interface for setting up the object, should be called when the object is enabled for the first time
/// Sample code:
/// <code>
/// void OnEnable(){
///     if (!IsSet) SetUp();
/// }
/// void SetUp(){
///     IsSet = true;
/// }
/// </code>
/// </summary>
public interface ISetUp
{
    bool IsSet { get; set; }
    void SetUp();
}