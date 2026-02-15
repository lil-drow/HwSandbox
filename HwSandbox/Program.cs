// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using HwSandbox.Abstractions;
using HwSandbox.Implementations;

Console.WriteLine("Проект для подготовки домашнего задания в рамках обучения на курсе Отус.");
IServiceCollection services = new ServiceCollection().AddTransient<DataWriter, TableWriter>()
                                                    .AddTransient<Homework, Homework_26>();

var serviceProvider = services.BuildServiceProvider();
Homework homework = serviceProvider.GetService<Homework>();
homework.Go();
