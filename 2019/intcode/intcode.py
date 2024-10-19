from enum import Enum
import queue


class IntcodeCond(Enum):
    TRUE = 1
    FALSE = 2
    LESS_THAN = 3
    EQUALS = 4


class Intcode:
    stdin = None
    stdout = None
    _program = []
    _ip = 0
    _relative_base = 0
    _done = False

    def __init__(self, program, stdin=queue.Queue(), stdout=queue.Queue()):
        assert isinstance(stdin, queue.Queue), "stdin argument is not a Queue object"
        assert isinstance(stdout, queue.Queue), "stdout argument is not a Queue object"

        self._program = program
        self.stdin = stdin
        self.stdout = stdout

    def run(self):
        while not self._done:
            self.step()

        return self._program[0]

    def step(self):
        instr = self._program[self._ip]
        mnemonic = instr % 100
        mode = int((instr - mnemonic) / 100)

        match mnemonic:
            case 1:
                self._add(mode)
            case 2:
                self._mult(mode)
            case 3:
                self._read_stdin(mode)
            case 4:
                self._write_stdout(mode)
            case 5:
                self._jump_cond(mode, IntcodeCond.TRUE)
            case 6:
                self._jump_cond(mode, IntcodeCond.FALSE)
            case 7:
                self._cond(mode, IntcodeCond.LESS_THAN)
            case 8:
                self._cond(mode, IntcodeCond.EQUALS)
            case 9:
                self._modify_relative_base(mode)
            case 99:
                self._done = True

    def _read_mem_mode(self, ip, mode):
        assert mode <= 2 and mode >= 0
        assert ip < len(self._program)

        dst_pointer = 0
        match mode:
            case 0:
                dst_pointer = self._program[ip]
            case 1:
                dst_pointer = ip
            case 2:
                dst_pointer = self._relative_base + self._program[ip]

        if dst_pointer >= len(self._program):
            self._extend_mem(dst_pointer - len(self._program) + 1)

        return self._program[dst_pointer]

    def _write_mem_mode(self, ip, mode, data):
        assert mode <= 2 and mode >= 0
        assert ip < len(self._program)

        dst_pointer = self._program[ip]
        if mode == 2:
            dst_pointer = self._program[ip] + self._relative_base

        if dst_pointer >= len(self._program):
            self._extend_mem(dst_pointer - len(self._program) + 1)

        self._program[dst_pointer] = data

    def _extend_mem(self, amount):
        self._program.extend(amount * [0])

    def _get_mode(self, modes, idx):
        return int(((modes % pow(10, idx+1)) - (modes % pow(10, idx))) / pow(10, idx))

    def _add(self, param_modes):
        src_val1 = self._read_mem_mode(self._ip + 1, self._get_mode(param_modes, 0))
        src_val2 = self._read_mem_mode(self._ip + 2, self._get_mode(param_modes, 1))
        self._write_mem_mode(self._ip + 3, self._get_mode(param_modes, 2), src_val1 + src_val2)
        self._ip += 4

    def _mult(self, param_modes):
        src_val1 = self._read_mem_mode(self._ip + 1, self._get_mode(param_modes, 0))
        src_val2 = self._read_mem_mode(self._ip + 2, self._get_mode(param_modes, 1))
        self._write_mem_mode(self._ip + 3, self._get_mode(param_modes, 2), src_val1 * src_val2)
        self._ip += 4

    def _read_stdin(self, param_modes):
        self._write_mem_mode(self._ip + 1, self._get_mode(param_modes, 0), self.stdin.get())
        self._ip += 2

    def _write_stdout(self, param_modes):
        src_val = self._read_mem_mode(self._ip + 1, self._get_mode(param_modes, 0))
        self.stdout.put(src_val)
        self._ip += 2

    def _jump_cond(self, param_modes, cond):
        param1 = self._read_mem_mode(self._ip + 1, self._get_mode(param_modes, 0))
        param2 = self._read_mem_mode(self._ip + 2, self._get_mode(param_modes, 1))

        if cond is IntcodeCond.TRUE:
            self._ip = param2 if param1 != 0 else self._ip + 3
        else:
            self._ip = param2 if param1 == 0 else self._ip + 3

    def _cond(self, param_modes, cond):
        param1 = self._read_mem_mode(self._ip + 1, self._get_mode(param_modes, 0))
        param2 = self._read_mem_mode(self._ip + 2, self._get_mode(param_modes, 1))

        data = 0
        if cond is IntcodeCond.LESS_THAN:
            data = 1 if param1 < param2 else 0
        else:
            data = 1 if param1 == param2 else 0
        self._write_mem_mode(self._ip + 3, self._get_mode(param_modes, 2), data)
        self._ip += 4

    def _modify_relative_base(self, param_modes):
        param = self._read_mem_mode(self._ip + 1, self._get_mode(param_modes, 0))
        self._relative_base += param
        self._ip += 2
