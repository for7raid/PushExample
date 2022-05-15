using PushExample.Abstraction;
using PushExample.Implementation;
using PushExample.Models;

var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.ModelBinderProviders.Insert(0, new MessageModelBinderProvider());
});

builder.Services.AddLogging(configure => configure.AddConsole());

builder.Services.AddHostedService<QueuedHostedService>();
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddSingleton<FakeHttpService>();
builder.Services.AddScoped<IMessageStore, InMemoryMessageStore>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<INotificationSenderProvider, NotificationSenderProvider>();
builder.Services.AddScoped<GoogleNotificationSender>();
builder.Services.AddScoped<AppleNotificationSender>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
