using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using Unity.Networking.Transport.Utilities;
using System.Resources;

public class BaseClient : MonoBehaviour
{
    public NetworkDriver driver;
    protected NetworkConnection connection;
    public Load load;

    public ResourceLoader resourceLoader;

    private void Start() { Init(); }
    private void Update() { UpdateServer(); }
    private void OnDestroy() { Shutdown(); }

    public virtual void Init()
    {
        //Initialize the driver

        var settings = new NetworkSettings();
        settings.WithFragmentationStageParameters(payloadCapacity: 10000);

        driver = NetworkDriver.Create(settings);

        var pipeline = driver.CreatePipeline(typeof(FragmentationPipelineStage), typeof(ReliableSequencedPipelineStage));
        connection = default(NetworkConnection);

        NetworkEndpoint endpoint = NetworkEndpoint.LoopbackIpv4;
        endpoint.Port = 63982;
        connection = driver.Connect(endpoint);
    }
    public virtual void Shutdown()
    {
        driver.Dispose();
    }
    public virtual void UpdateServer()
    {
        driver.ScheduleUpdate().Complete();
        CheckAlive();
        UpdateMessagePump();
    }

    private void CheckAlive()
    {
        if(!connection.IsCreated)
        {
            NativeWinAlert.Show("Error", "Failed Connecting to local Server", "OK");
        }
    }
    protected virtual void UpdateMessagePump()
    {
        DataStreamReader streamReader;

        NetworkEvent.Type cmd;

        while ((cmd = connection.PopEvent(driver, out DataStreamReader stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                NativeWinAlert.Show("Successfully connected to server.", "Notice", "OK");
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                NativeWinAlert.Show("Recieved data from server ", "Data", "OK");
                OnData(stream);

            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                NativeWinAlert.Show("Client has lost connection to the server", "Notice", "OK");
            }
        }
    }
    public virtual void OnData(DataStreamReader streamReader)
    {
        NetMessage msg = null;
        OpCode opCode = (OpCode)streamReader.ReadUInt();

        switch (opCode)
        {
            case OpCode.CONSOLE_LOG:
                msg = new NetConsoleMessage(streamReader);
                NetConsoleMessage netConsoleMessage = (NetConsoleMessage)msg;

                load.inAccessible.SetActive(false);

                NativeWinAlert.Show(netConsoleMessage.RecievedLog, "Data", "OK");
                break;
            case OpCode.LOAD_FILE:
                msg = new NetLoader(streamReader);
                NetLoader netLoader = (NetLoader)msg;

                load.LoadNET(netLoader.RecievedPaths);
                break;
            case OpCode.RESOURCE_LOAD:
                msg = new NetResourceLoad(streamReader);
                NetResourceLoad netResourceLoad = (NetResourceLoad)msg;

                resourceLoader.NetLoadResource(netResourceLoad.RecievedLog);
                break;
            default:
                NativeWinAlert.Show("No OpCode recieved from client.", "Error", "OK");
                break;
        }
    }
    public virtual void SendToServer(NetMessage netMessage)
    {
        DataStreamWriter streamWriter;
        driver.BeginSend(connection, out streamWriter);
        netMessage.Serialize(ref streamWriter);
        driver.EndSend(streamWriter);
    }
}
