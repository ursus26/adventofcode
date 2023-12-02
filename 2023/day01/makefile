CC=gcc
CFLAGS=-Wall -Werror=format-security -Werror=implicit-function-declaration -pipe -fasynchronous-unwind-tables -g

build:
	@$(CC) $(CFLAGS) main.c -o main

run: build
	@./main

clean:
	@rm -f ./main ./main.o