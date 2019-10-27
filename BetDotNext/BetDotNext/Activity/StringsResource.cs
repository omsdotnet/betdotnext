namespace BetDotNext.Activity
{
  public static class StringsResource
  {
    public static string StartActivityMessage = "Привет!!!. Я бот который принимает ставки на DotNext Moscow 2019.";

    public static readonly string AcceptedBetActivityMessage = "Формат принятия ставки следующий: \r\n[спикер]-[ставка]-[номинация]";

    public static readonly string LoadingMessage = "Ща как поставлю. Ты красавчик!!!";

    public static readonly string BetActivityUnexpectedFormatMessage = "Не верный формат ставки";

    public static readonly string SuccessBetActivity = "Ставка успешно принята. \r\nТекущее количество очков {0}.";

    public static readonly string FailCreatedActivityMessage = "Не удалось создать ставку.\r\nПопробуйте выполнить команду снова.";

    public static readonly string RemoveBetActivityMessage = "Данная команда предпологает следующий формат снятия ставки:\r\n" + 
                                                             "[участник]-[спикер]-0-[номинация]\r\n" +
                                                             "[участник]-[спикер]-0 (снять все ставки на спикера)\r\n" +
                                                             "[участник]-0 (снять все текущие ставки участника)";
  }
}
