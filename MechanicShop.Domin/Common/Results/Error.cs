namespace MechanicShop.Domin.Common.Results
{
    public readonly record struct Error  // why record struct ? because we want to make it immutable and value type for better performance and memory efficiency.

    {

        private Error(string code, string description, ErrorKind type)
        { 
            Code = code;
            Description = description;
            Type = type;

        }
    
        public string Code { get; }
        public string Description { get; }
        public ErrorKind Type { get; }

        public static Error Failure(string code = nameof(Failure), string description = "General failure") => new(code, description, ErrorKind.Failure);
    
        public static Error Unexpected(string code = nameof(Unexpected), string description = "Unexpected  failure") => new(code, description, ErrorKind.Unexpected);

        public static Error Validation(string code = nameof(Validation), string description = "Validation failure") => new(code, description, ErrorKind.Validation);

        public static Error Conflict(string code = nameof(Conflict), string description = "Conflict failure") => new(code, description, ErrorKind.Conflict);

        public static Error NotFound(string code = nameof(NotFound), string description = "Not found failure") => new(code, description, ErrorKind.NotFound);
    
        public static Error Unauthorized(string code = nameof(Unauthorized), string description = "Unauthorized failure") => new(code, description, ErrorKind.Unauthorized);
    
       public static Error Forbidden(string code = nameof(Forbidden), string description = "Forbidden failure") => new(code, description, ErrorKind.Forbidden);

        public static Error Create (string code, string description, int  type) => new(code, description, (ErrorKind)type);
    }
}   
