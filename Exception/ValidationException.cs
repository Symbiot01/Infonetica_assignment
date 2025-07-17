
using System;



namespace WorkflowService.Exceptions;



public class ValidationException : Exception

{

    public ValidationException(string message) : base(message) { }

}