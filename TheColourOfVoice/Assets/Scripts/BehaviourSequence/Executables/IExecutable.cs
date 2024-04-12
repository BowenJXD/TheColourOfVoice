using System.Collections;
using System.Collections.Generic;

public interface IExecutable
{
    public IEnumerator Execute(IExecutor newExecutor);
}

public interface IExecutor
{
    public Dictionary<string, object> Blackboard { get; set; }
}