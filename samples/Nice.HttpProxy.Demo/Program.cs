using Nice.HttpProxy;
using Nice.HttpProxy.Abstractions;
using System;
using System.Diagnostics;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpForwarder(options => {
    options.MaxRetries = 3;
    options.ShadowSendExceptionHandler = async (url, times,request, error) =>
    {
        await global::System.Console.Out.WriteLineAsync($"��ַ:{url}����������{times}���쳣 ԭ��:{error}");
        await ValueTask.CompletedTask;
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.


app.Run(async (ctx) =>
{
    ctx.Request.EnableBuffering();
    var forward = ctx.RequestServices.GetRequiredService<IHttpForwarder>();
    await forward.SendAsync(ctx, new[] { "http://demo1.example.com", "http://demo2.example.com" }, (u, m) =>
    {
        global::System.Console.WriteLine("�˴����Ե���������ز�����ע�⣺һ���޸��������Ӱ��������Ч(ע�ⲻҪ�ظ�����)");
        return ValueTask.CompletedTask;
    });
});

app.Run();
Console.WriteLine( "");
