namespace Olbrasoft.Data.Cqrs;
/// <summary>
/// https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
/// </summary>
public enum CommandStatus
{
    Default = 0,
    Success = 200,
    Created = 201,
    Accepted = 202,
    Deleted = 204,
    Modified = 302,
    Unchanged = 304,
    NotFound = 404,
    Conflict = 409,
    Removed = 410,
    Added = 206,
    Error = 500
}
