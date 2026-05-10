namespace TinyViz.Contracts.Model.Exceptions;

public class ConverterException(string message, Exception? innerException = null) : TinyVizException(message, innerException);
