using System;

public class TooltipEnableEventArgs : EventArgs {
  public string title;
  public string subtitle;

  public TooltipEnableEventArgs(string title, string subtitle) {
    this.title = title;
    this.subtitle = subtitle;
  }
}
