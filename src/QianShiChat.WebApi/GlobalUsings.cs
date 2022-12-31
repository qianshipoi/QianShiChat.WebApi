global using AutoMapper;

global using EasyCaching.Core;
global using EasyCaching.Serialization.SystemTextJson.Configurations;

global using FluentValidation;

global using MediatR;

global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Controllers;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.AspNetCore.StaticFiles;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.FileProviders;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using Microsoft.Extensions.Options;

global using QianShiChat.Application.Contracts;
global using QianShiChat.Application.Hubs;
global using QianShiChat.Application.Services;
global using QianShiChat.Common.Extensions;
global using QianShiChat.Common.Helpers;
global using QianShiChat.Domain;
global using QianShiChat.Domain.Core.AppOops;
global using QianShiChat.Domain.Core.Interceptors;
global using QianShiChat.Domain.Models;
global using QianShiChat.Domain.Options;
global using QianShiChat.Domain.Shared;
global using QianShiChat.WebApi.BackgroundHost;
global using QianShiChat.WebApi.Filters;

global using QRCoder;

global using Quartz;

global using SixLabors.ImageSharp.Web.Caching;
global using SixLabors.ImageSharp.Web.Commands;
global using SixLabors.ImageSharp.Web.DependencyInjection;
global using SixLabors.ImageSharp.Web.Processors;
global using SixLabors.ImageSharp.Web.Providers;

global using System.ComponentModel.DataAnnotations;
global using System.Globalization;
global using System.Reflection;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;

global using Yitter.IdGenerator;