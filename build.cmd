@echo off
cls

dotnet restore
dotnet build %*
