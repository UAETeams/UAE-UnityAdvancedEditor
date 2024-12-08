using Unity.Collections;

public class NetMessage
{
    public OpCode Code { get; set; }

    public virtual void Serialize(ref DataStreamWriter streamWriter)
    {

    }
    public virtual void DeSerialize(DataStreamReader streamReader)
    {

    }
}
