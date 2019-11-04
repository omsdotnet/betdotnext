namespace BetDotNext.Activity
{
  public static class StringsResource
  {
    public static string StartActivityMessage = "Здравия желаю!!!\r\n" + 
                                                "Я бот, который принимает ставки на рейтинг спикеров DotNext.\r\n" +
                                                "Спикеры: https://dotnext-moscow.ru/people/ \r\n" +
                                                "Результаты ставок: http://bookmakerboard.azurewebsites.net/ \r\n" +
                                                "Чат для обсуждения и вопросов: https://t.me/dotnext_rates" +
                                                "\r\n" +
                                                "У Вас есть 1000 баллов, которые Вы можете ставить на то, что спикер попадет в номинации рейтинга: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, top3, top5, top10\r\n" +
                                                "\r\n" +
                                                "После публикации организаторами рейтинга спикеров на хабре, если Ваша ставка сработала, то:\r\n" +
                                                "количество баллов в категории top10 - удваивается\r\n" +
                                                "количество баллов в категории top5 - утраивается\r\n" +
                                                "количество баллов в категории top3 - учетверяется\r\n" +
                                                "количество баллов поставленных на определенные места (1-10) - упятеряется\r\n" +
                                                "\r\n" +
                                                "победитель, набравший в итоге больше всего баллов, получает приз - https://www.amazon.com/Pro-NET-Benchmarking-Performance-Measurement/dp/1484249402 \r\n" +
                                                "за 2 и 3 места - сертификаты на продукты JetBrains \r\n" +
                                                "при равном колличестве итоговых баллов, преимущество получают ставки сделанные раньше по времени \r\n";

    public static readonly string AcceptedBetActivityMessage = "Формат принятия ставки следующий: \r\n" +
                                                               "[спикер] - [ставка] - [номинация] \r\n" +
                                                               "Например:\r\n" +
                                                               "Christophe Nasarre - 300 - 1\r\n" +
                                                               "Роман Просин - 100 - top3\r\n" +
                                                               "tukasz Pyrzyk - 10 - top10\r\n" +
                                                               "\r\n" +
                                                               "повторная ставка на спикера в той-же номинации, считается корретировкой предыдущей ставки";

    public static readonly string LoadingMessage = "Обрабатываю Вашу команду...";

    public static readonly string BetActivityUnexpectedFormatMessage = "Неверный формат ставки";

    public static readonly string SuccessBetActivity = "Команда успешно обработана.\r\n" + 
                                                       "Осталось баллов: {0}";

    public static readonly string FailCreatedActivityMessage = "Не удалось создать ставку.\r\n" + 
                                                               "Попробуйте выполнить команду снова";

    public static readonly string FailDeleteActivityMessage = "Не удалось удалить ставку.\r\n" + 
                                                              "Попробуйте выполнить команду снова";

    public static readonly string RemoveBetActivityMessage = "Формат снятия ставки слядующий:\r\n" + 
                                                             "[спикер] - 0 - [номинация]\r\n" +
                                                             "[спикер] - 0   (снять все ставки на спикера)";

    public static readonly string BetRateNotEquelsMessage = "Ставка должна быть равна 0.";

    public static readonly string SuccessfullyRemoveMessage = "Ваша ставка успешно удалена.";

    public static readonly string NotExistingBidderMessage = "Сначало вы должны сделать ставку.";

    public static readonly string CurrentScoreRemoveMessage = "Все ваши ставки успешно удалены.\r\n" +
                                                              "Осталось баллов: {0}";

    public static readonly string ExistingSpeakerMessage = "Указанный спикер не существует.";

    public static readonly string HelpText = "Бот понимает команды:\r\n" +
                                             "/start - начало работы с ботом\r\n" +
                                             "/bet - сделать ставку на спикера\r\n" +
                                             "/removebet - снять поставленную ранее ставку\r\n" +
                                             "/removeall - снять все ранее поставленные ставки\r\n" +
                                             "/score - отобразить свой текущий счет\r\n" +
                                             "/help - выдать эту справку";

    public static readonly string IncorectNomination = "Неверный формат номинации.";

    public static readonly string NominationNotEmpty = "Номинация не может быть пустой.";

    public static readonly string GettingCurrentScoreException = "Не удалось получить текущее состояние счёта.";

    public static readonly string CurrentScoreMessage = "Осталось баллов: {0}.";
  }
}
