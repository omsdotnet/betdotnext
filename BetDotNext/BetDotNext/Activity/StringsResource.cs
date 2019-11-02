namespace BetDotNext.Activity
{
  public static class StringsResource
  {
    public static string StartActivityMessage = "Привет! Я бот, который принимает ставки на рейтинг спикеров DotNext\r\n" +
                                                "Спикеры: https://dotnext-moscow.ru/people/ \r\n" +
                                                "Результаты ставок: http://bookmakerboard.azurewebsites.net/";

    public static readonly string AcceptedBetActivityMessage = "Формат принятия ставки следующий: \r\n" +
                                                               "[спикер]-[ставка]-[номинация] \r\n" +
                                                               "Например:\r\n" +
                                                               "Christophe Nasarre - 300 - 1\r\n" +
                                                               "Роман Просин - 100 - top3\r\n" +
                                                               "tukasz Pyrzyk - 10 - top10";

    public static readonly string LoadingMessage = "Обрабатываю Вашу ставку...";

    public static readonly string BetActivityUnexpectedFormatMessage = "Не верный формат ставки";

    public static readonly string SuccessBetActivity = "Ставка успешно принята.\r\n" + 
                                                       "Текущее количество баллов {0}";

    public static readonly string FailCreatedActivityMessage = "Не удалось создать ставку.\r\n" + 
                                                               "Попробуйте выполнить команду снова";

    public static readonly string FailDeleteActivityMessage = "Не удалось удалить ставку.\r\n" + 
                                                              "Попробуйте выполнить команду снова";

    public static readonly string RemoveBetActivityMessage = "Данная команда предпологает следующий формат снятия ставки:\r\n" + 
                                                             "[спикер] - 0 - [номинация]\r\n" +
                                                             "[спикер] - 0 (снять все ставки на спикера)\r\n" +
                                                             "0 (снять все текущие ставки участника)";

    public static readonly string BetRateNotEquelsMessage = "Ставка должна быть равна 0";

    public static readonly string SuccessfullyRemoveMessage = "Ваша ставка успешно удалена";
  }
}
