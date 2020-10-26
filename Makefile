
clean: 
	rm -rf ./bin
	rm -rf ./obj

build: clean
	dotnet build

run: build
	dotnet run
