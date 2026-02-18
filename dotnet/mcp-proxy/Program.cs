// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Agents.A365.Tooling.LocalMcp.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Local MCP Proxy services (WNS, session management, etc.)
// Configuration is read from appsettings.json sections: WnsConfiguration, LocalMcp
builder.Services.AddLocalMcpProxy(builder.Configuration);

var app = builder.Build();

// Map all Local MCP Proxy endpoints:
//   POST /api/channels/register   - Desktop WNS channel registration
//   GET  /api/channels            - List registered channels
//   POST /api/notify/{clientName} - Trigger WNS push to desktop
//   WS   /ws/mcp/{sessionId}     - WebSocket bridge (desktop connects here)
//   POST /api/mcp/{sessionId}    - HTTP→WS relay (agent sends MCP requests here)
//   POST /api/discovery/{id}/servers - Desktop posts discovered servers
//   GET  /api/discovery/{id}/servers - Agent polls for discovered servers
//   GET  /api/status/{sessionId} - Session status polling
//   POST /api/heartbeat/{sessionId} - Session keepalive
app.UseLocalMcpProxy();

// Health check endpoint
app.MapGet("/", () => "MCP Proxy is running");
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

app.Run();
