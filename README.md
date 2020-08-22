# TwitchApps

Twitch API is a class library developed to provide easy access to developer endpoints of twitch.tv.  It currently only provides the functionality required for another application I wrote.
## Creating a Client

```csharp
TwitchClient client = Twitch.Start("<Your Application ID>",
                                   "<Your Client Secret>",
                                   "<Your Redirect URL>",
                                    new string[]
                                    {
                                        "RequiredScope1",
                                        "RequiredScope2",
                                        ...                                                  
                                    });
```

## Commands and Queries

The API uses a command and query model to send commands and request info from Twitch

```csharp
var command = new UpdateChannelInfoCommand(channelID, "channelStatus", "gameTitle");
await client.V5.ExecuteCommandAsync(command);

var query = new GetGamesByNameQuery("searchString");
GetGamesByNameResponse response = await client.V5.ExecuteQueryAsync(query);
```
