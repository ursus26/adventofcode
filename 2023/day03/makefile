CC:=gcc
CFLAGS:=-Wall -Werror=format-security -Werror=implicit-function-declaration -pipe -fasynchronous-unwind-tables -g
OBJ:=main.o list.o
HEADER:=list.h
TARGET:=day03


%.o: %.c $(HEADER)
	$(CC) -c -o $@ $< $(CFLAGS)

.PHONY: build
build: $(OBJ)
	$(CC) -o $(TARGET) $^ $(CFLAGS)

.PHONY: run
run: build
	@./$(TARGET)

.PHONY: clean
clean:
	@rm -f $(TARGET) $(OBJ)