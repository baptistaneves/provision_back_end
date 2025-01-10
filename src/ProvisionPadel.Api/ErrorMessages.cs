namespace ProvisionPadel.Api;

public static class ErrorMessages
{
    public const string ChannelIsRecording = "Esta câmara já está em gravação";
    public const string ErrorStartingRecord = "Ocorreu um erro ao iniciar gravação. Verifique o equipamento de gravação";
    public const string ErrorStopRecording = "Ocorreu um erro ao parar a gravação. Verifique o equipamento de gravação";
    public const string ErrorDownloadingVideo = "Ocorreu um erro ao baixar o vídeo";
    public const string CameraNotFound = "Nenhuma camara foi encontrada com o ID solicitado";
    public const string CameraRecordingCanNotBeRemoved = "Está camara está em garavação, não pode ser removida";
    public const string CameraWithVideoCanNotBeRemoved = "Está camara está possui videos gravados, não pode ser removida";

}
