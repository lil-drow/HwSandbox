// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using HwSandbox;
using HwSandbox.Abstractions;
using HwSandbox.Implementations;

Console.WriteLine("Проект для подготовки домашнего задания в рамках обучения на курсе Отус.");
IServiceCollection services = new ServiceCollection().AddTransient<IDataWriter, TableWriter>()
                                                    .AddTransient<IHomework, Homework_15>();

var serviceProvider = services.BuildServiceProvider();
IHomework homework = serviceProvider.GetService<IHomework>();
homework.Go();
