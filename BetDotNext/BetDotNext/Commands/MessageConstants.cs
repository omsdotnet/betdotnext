using System;

namespace BetDotNext.Commands
{
  public static class MessageConstants
  {
    public static readonly string BetCreateMessage = "Формат принятия ставки следующий: " + 
                                              Environment.NewLine +
                                              "[спикер]-[ставка]-[номинация]";

    public static readonly string WrongCreateDataMessage = "Неправильный формат данных. Попробуйте повторить снова.";
  }
}
