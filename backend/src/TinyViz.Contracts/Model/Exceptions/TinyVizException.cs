namespace TinyViz.Contracts.Model.Exceptions;

public class TinyVizException(string message, Exception? innerException = null) : Exception(message, innerException);
