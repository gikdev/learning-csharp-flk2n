# Install it first:
#
# dotnet tool install --global httpgenerator

httpgenerator http://localhost:5023/openapi/v1.json --skip-validation -o ./HttpFiles/;
