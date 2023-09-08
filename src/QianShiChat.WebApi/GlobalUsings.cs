global using AutoMapper;

global using EasyCaching.Core;
global using EasyCaching.Serialization.SystemTextJson.Configurations;

global using FluentValidation;

global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.HttpOverrides;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Controllers;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.FileProviders;
global using Microsoft.Extensions.Options;
global using Microsoft.Extensions.Primitives;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Any;
global using Microsoft.OpenApi.Models;

global using QianShiChat.Application.Common.Interfaces;
global using QianShiChat.Application.Contracts;
global using QianShiChat.Application.Filters;
global using QianShiChat.Application.Hubs;
global using QianShiChat.Application.Services;
global using QianShiChat.Domain;
global using QianShiChat.Domain.Core.AppOops;
global using QianShiChat.Domain.Extensions;
global using QianShiChat.Domain.Models;
global using QianShiChat.Domain.Options;
global using QianShiChat.Domain.Shared;
global using QianShiChat.WebApi.BackgroundHost;
global using QianShiChat.WebApi.Extensions;
global using QianShiChat.WebApi.Filters;
global using QianShiChat.WebApi.Helpers;

global using QRCoder;

global using Quartz;

global using SixLabors.ImageSharp.Web.Caching;
global using SixLabors.ImageSharp.Web.Commands;
global using SixLabors.ImageSharp.Web.DependencyInjection;
global using SixLabors.ImageSharp.Web.Processors;
global using SixLabors.ImageSharp.Web.Providers;

global using Swashbuckle.AspNetCore.SwaggerGen;

global using System.ComponentModel.DataAnnotations;
global using System.Globalization;
global using System.Net;
global using System.Reflection;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;

global using tusdotnet;
global using tusdotnet.Interfaces;
global using tusdotnet.Models;
global using tusdotnet.Models.Concatenation;
global using tusdotnet.Models.Configuration;
global using tusdotnet.Models.Expiration;
global using tusdotnet.Stores;

global using Yitter.IdGenerator;
